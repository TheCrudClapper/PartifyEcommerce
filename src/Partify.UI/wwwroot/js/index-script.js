window.addEventListener('DOMContentLoaded', () => {
    const setupScroll = (carouselId, leftBtnId, rightBtnId) => {
        const container = document.getElementById(carouselId);
        if (!container) return;

        const item = container.querySelector('.flex-shrink-0');
        if (!item) return;

        const itemWidth = item.offsetWidth;

        document.getElementById(leftBtnId)?.addEventListener('click', () => {
            container.scrollBy({ left: -itemWidth, behavior: 'smooth' });
        });

        document.getElementById(rightBtnId)?.addEventListener('click', () => {
            container.scrollBy({ left: itemWidth, behavior: 'smooth' });
        });
    };
    setupScroll('featuredCarousel', 'featuredScrollLeft', 'featuredScrollRight');
    setupScroll('freshCarousel', 'freshScrollLeft', 'freshScrollRight');
    setupScroll('categoryCarousel', 'categoryScrollLeft', 'categoryScrollRight');
    setupScroll('dealsCarousel', 'dealsScrollLeft', 'dealsScrollRight');
});

window.onload = function () {
    startTime();
};

function startTime() {
    const today = new Date();
    let h = today.getHours();
    let m = today.getMinutes();
    let s = today.getSeconds();

    let hoursLeft = 23 - h;
    let minutesLeft = 59 - m;
    let secondsLeft = 59 - s;

    minutesLeft = formatTime(minutesLeft);
    secondsLeft = formatTime(secondsLeft);

    document.getElementById("clock").innerHTML = hoursLeft + ":" + minutesLeft + ":" + secondsLeft;
    setTimeout(startTime, 1000);

}

function formatTime(i) {
    return (i < 10 ? "0" + i : i);
}