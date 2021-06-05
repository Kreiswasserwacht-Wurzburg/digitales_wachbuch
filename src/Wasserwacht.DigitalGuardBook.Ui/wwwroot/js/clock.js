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
    return (dateObject.getHours() + ":" + dateObject.getMinutes() + ":" + dateObject.getSeconds());
}

docReady(function () {
    // DOM is loaded and ready for manipulation here
    setInterval(function () {
        var clockEle = document.getElementById("clock");
        clockEle.textContent = formatDate(new Date());
    }, 1000);
});