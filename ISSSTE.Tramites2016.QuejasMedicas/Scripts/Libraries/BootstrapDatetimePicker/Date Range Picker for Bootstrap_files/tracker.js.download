function w3_inject(url) {
    var n = document.createElement("script");
    n.type = "text/javascript";
    n.src = url;
    n.async = true;
    var r = document.getElementsByTagName("script")[0];
    r.parentNode.insertBefore(n, r);
}

function w3counter(id) {
    
    if (typeof window._w3counter !== 'undefined') return;

    var info = '&userAgent=' + encodeURIComponent(navigator.userAgent);
    info += '&webpageName=' + encodeURIComponent(document.title);
    info += '&ref=' + encodeURIComponent(document.referrer);
    info += '&url=' + encodeURIComponent(window.location);
    info += '&width=' + screen.width;
    info += '&height=' + screen.height;
    info += '&rand=' + Math.round(1000*Math.random());

    document.write('<a href="http://www.w3counter.com"><img src="https://www.w3counter.com/tracker.php?id=' + id + info + '" style="border: 0" alt="W3Counter Web Stats" /></a>');

    w3_inject('https://pulse.w3counter.com/pulse.js?id=' + id);

    window._w3counter = true;

}

