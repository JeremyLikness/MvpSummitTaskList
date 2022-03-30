(async function () {

    window.db = {
        init: false,
        data: null
    };

    window.db.synchronizeDbWithCache = async function (filename) {

        const backupPath = `/${filename}_bak`;

        if (!window.db.init) {

            window.db.init = true;

            console.log("Checking cache...");

            const cache = new Promise((res, err) => {
                FS.loadFilesFromDB([backupPath], _ => {
                    res(1);
                },
                    loadErr => err(loadErr));
            });

            let restored = await cache;

            if (restored == 1) {
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

            const restore = new Promise((res, err) => {
                FS.saveFilesToDB([backupPath], _ => {
                    res(1);
                },
                    loadErr => err(loadErr));
            });

            const result = await restore;

            if (result == 1) {
                console.log("Data cached.");
                return 1;
            }
        }
        else {
            console.log("File not found.");
        }
        return -1;
    };
})();