window.setLang = function (src, trg) {
    const value = `/${src}/${trg}`;
    document.cookie = `googtrans=${value}; path=/`;
    document.cookie = `googtrans=${value}; path=/; domain=${location.hostname}`;
};