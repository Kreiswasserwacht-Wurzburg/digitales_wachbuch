function docReady(fn) {
    // see if DOM is already available
    if (document.readyState === "complete" || document.readyState === "interactive") {
        // call on next available tick
        setTimeout(fn, 1);
    } else {
        document.addEventListener("DOMContentLoaded", fn);
    }
}

function formatDate(dateObject) //pass date object
{
    return dateObject.toLocaleDateString('de-DE', { weekday: 'long', year: 'numeric', month: 'short', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' });
}

docReady(function () {
    // DOM is loaded and ready for manipulation here
    setInterval(function () {
        var clockEle = document.getElementById("clock");
        clockEle.innerHTML = formatDate(new Date()).replace(',', '<br />').replace(',', '<br />').replace("<br />", ',');
    }, 1000);
});