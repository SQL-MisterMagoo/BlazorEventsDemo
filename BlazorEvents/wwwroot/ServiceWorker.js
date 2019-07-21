const filesToCache = [
"/_framework/blazor.boot.json",
"/_framework/blazor.server.js",
"/_framework/blazor.webassembly.js",
"/_framework/wasm/mono.js",
"/_framework/wasm/mono.wasm",
"/_framework/_bin/BlazorEvents.dll",
"/_framework/_bin/BlazorEvents.pdb",
"/_framework/_bin/Microsoft.AspNetCore.Authorization.dll",
"/_framework/_bin/Microsoft.AspNetCore.Blazor.dll",
"/_framework/_bin/Microsoft.AspNetCore.Components.Browser.dll",
"/_framework/_bin/Microsoft.AspNetCore.Components.dll",
"/_framework/_bin/Microsoft.AspNetCore.Metadata.dll",
"/_framework/_bin/Microsoft.Extensions.DependencyInjection.Abstractions.dll",
"/_framework/_bin/Microsoft.Extensions.DependencyInjection.dll",
"/_framework/_bin/Microsoft.Extensions.Logging.Abstractions.dll",
"/_framework/_bin/Microsoft.Extensions.Options.dll",
"/_framework/_bin/Microsoft.Extensions.Primitives.dll",
"/_framework/_bin/Microsoft.JSInterop.dll",
"/_framework/_bin/Mono.Security.dll",
"/_framework/_bin/Mono.WebAssembly.Interop.dll",
"/_framework/_bin/mscorlib.dll",
"/_framework/_bin/System.Buffers.dll",
"/_framework/_bin/System.ComponentModel.Annotations.dll",
"/_framework/_bin/System.Core.dll",
"/_framework/_bin/System.dll",
"/_framework/_bin/System.Memory.dll",
"/_framework/_bin/System.Net.Http.dll",
"/_framework/_bin/System.Numerics.Vectors.dll",
"/_framework/_bin/System.Runtime.CompilerServices.Unsafe.dll",
"/_framework/_bin/System.Text.Json.dll",
"/_framework/_bin/System.Threading.Tasks.Extensions.dll",
"/css/bootstrap/bootstrap.min.css",
"/css/bootstrap/bootstrap.min.css.map",
"/css/Events.css",
"/css/open-iconic/FONT-LICENSE",
"/css/open-iconic/font/css/open-iconic-bootstrap.min.css",
"/css/open-iconic/font/fonts/open-iconic.eot",
"/css/open-iconic/font/fonts/open-iconic.otf",
"/css/open-iconic/font/fonts/open-iconic.svg",
"/css/open-iconic/font/fonts/open-iconic.ttf",
"/css/open-iconic/font/fonts/open-iconic.woff",
"/css/open-iconic/ICON-LICENSE",
"/css/open-iconic/README.md",
"/css/site.css",
"/images/android-icon-192x192.png",
"/images/android-icon-512x512.png",
"/index.html",
"/manifest.json",
"/sample-data/weather.json",
"/ServiceWorker.js",
"/ServiceWorkerRegister.js"
];
const staticCacheName = 'cache-blazorevents-v1';
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
