window.getScrollData = function(element) {
    return {
        scrollTop: element.scrollTop,
        scrollHeight: element.scrollHeight,
        clientHeight: element.clientHeight
    };
};

window.setupScrollListener = function (element, dotnetRef) {
    let isChecking = false;

    element.addEventListener('scroll', async function () {
        if (isChecking) return;

        const scrollTop = element.scrollTop;
        const scrollHeight = element.scrollHeight;
        if (scrollTop > 0) {
            element.classList.remove("top");
        }
        else {
            element.classList.add("top");
        }

        const clientHeight = element.clientHeight;

        if (scrollHeight - scrollTop - clientHeight < 100) {
            isChecking = true;
            await dotnetRef.invokeMethodAsync('OnScrollReachedBottom');
            setTimeout(() => { isChecking = false; }, 500);
        }
    });
};

window.setScrollToTop = function (element) {
    element.scrollTop = 0;
    element.classList.add("top");
}