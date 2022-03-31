(async function () {

    const db = {
        init: false,
        cache: await caches.open('MvpSummitCache')
    };

    window.db = db;

    db.synchronizeDbWithCache = async function (filename) {

        const backupPath = `/${filename}`;
        const cachePath = `/data/cache/${backupPath.split('.')[0]}.db`;
        console.log(`Processing ${backupPath}...`);

        if (!db.init) {

            db.init = true;

            console.log("Checking cache...");

            const resp = await db.cache.match(cachePath);

            if (resp && resp.ok) {

                const res = await resp.arrayBuffer();

                if (res) {
                    FS.writeFile(backupPath, new Uint8Array(res));
                    const size = FS.stat(backupPath).size;
                    console.log(`Restored ${size} bytes from cache.`);
                    return 0;
                }
            }
            console.log("No cache available.");
            return -1;
        }

        if (FS.analyzePath(backupPath).exists) {

            // give files time to flush
            const waitFlush = new Promise((done, _) => {
                setTimeout(done, 10);
            });

            await waitFlush;

            const data = FS.readFile(backupPath);

            const blob = new Blob([data], {
                type: 'application/octet-stream',
                ok: true,
                status: 200
            });

            const headers = new Headers({
                'content-length': blob.size
            });

            const response = new Response(blob, {
                headers
            });

            await db.cache.put(cachePath, response);

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