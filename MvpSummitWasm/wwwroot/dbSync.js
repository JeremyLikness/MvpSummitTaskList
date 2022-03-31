(async function () {

    const db = {
        init: false,
        DB_NAME: 'SummitCache',
        DB_VERSION: 1,
        DB_STORE_NAME: 'Files',
        DB_KEY: 'File'
    };

    window.db = db;

    db.synchronizeDbWithCache = async function (filename) {

        const dbinit = action => new Promise((res, _) => {

            db.idxdb = FS.indexedDB().open(db.DB_NAME, db.DB_VERSION);

            db.idxdb.onupgradeneeded = () => {
                db.idxdb.result.createObjectStore(db.DB_STORE_NAME, { keypath: 'id' });
                db.idxdb.onupgradeneeded = () => { };
            };

            db.idxdb.onsuccess = () => {
                action(res);
            };
        });

        const backupPath = `/${filename}`;
        console.log(`Processing ${backupPath}...`);
        if (!db.init) {

            db.init = true;

            console.log("Checking cache...");

            const res = await dbinit(res => {
                const req = db.idxdb.result.transaction(db.DB_STORE_NAME, 'readonly')
                    .objectStore(db.DB_STORE_NAME)
                    .get(db.DB_KEY);

                req.onsuccess = () => {
                    res(req.result);
                };
            });

            if (res) {
                FS.createDataFile('/', backupPath.substring(1), res, true, true, true);
                const size = FS.stat(backupPath).size;
                console.log(`Restored ${size} bytes from cache.`);
                return 0;
            }
            else {
                console.log("No cache available.");
                return -1;
            }
        }

        if (FS.analyzePath(backupPath).exists) {

            // give files time to flush
            const waitFlush = new Promise((done, _) => {
                setTimeout(done, 10);
            });

            await waitFlush;

            const data = FS.readFile(backupPath);

            await dbinit(res => {
                db.idxdb.result.transaction(db.DB_STORE_NAME, 'readwrite')
                    .objectStore(db.DB_STORE_NAME)
                    .put(data, db.DB_KEY);
                res();
            });

            console.log("Data cached.");
            FS.unlink(backupPath);
            const exists = FS.analyzePath(backupPath).exists;
            console.log(`${backupPath} exists: ${exists}`);
            return 1;
        }
        else {
            console.log("File not found.");
        }
        return -1;
    };
})();