self.addEventListener('install', event => {
    self.skipWaiting();
    event.waitUntil(
        caches.open(staticCacheName)
            .then(cache => {
                return cache.addAll(filesToCache);
            })
    );
});
self.addEventListener('fetch', event => {
    const requestUrl = new URL(event.request.url);
    if (requestUrl.origin === location.origin) {
        if (requestUrl.pathname === '/') {
            event.respondWith(caches.match('/index.html'));
            return;
        }
    }
    event.respondWith(
        caches.match(event.request)
            .then(response => {
                if (response) {
                    return response;
                }
                return fetch(event.request)
                    .then(response => {
                        if (response.ok) {
                            if (requestUrl.origin === location.origin) {
                                return caches.open(staticCacheName).then(cache => {
                                    cache.put(event.request.url, response.clone());
                                });
                            }
                        }
                        return response;
                    });
            }).catch(error => {
                console.error(error);
            })
    );
});
