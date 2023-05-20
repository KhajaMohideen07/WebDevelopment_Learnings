const movetop = document.querySelector('.btn-top');

window.addEventListener('scroll', () => {
    var y = window.scrollY;
    if (y >= 600) {
        movetop.classList.add('active');
    }
    else {
        movetop.classList.remove('active');
    }
});