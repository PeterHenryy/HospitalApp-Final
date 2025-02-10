const burgerMenu = document.getElementById('burger-menu');
const navbar = document.getElementById('navbar');

burgerMenu.addEventListener('click', () => {
    navbar.classList.toggle('active');
});