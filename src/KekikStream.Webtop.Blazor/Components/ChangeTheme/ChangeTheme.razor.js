$(function () {
    function changeTheme(theme) {
        window.localStorage.setItem('theme', theme);

        document.getElementsByTagName('body')[0].setAttribute('data-bs-theme', theme);

        if (theme == "dark") {
            document.getElementById('ToolbarChangeThemeIcon').className = 'fas fa-sun';
        } else {
            document.getElementById('ToolbarChangeThemeIcon').className = 'fas fa-moon';
        }
    }

    function toggleTheme() {
        getTheme() == 'light' ? changeTheme('dark') : changeTheme('light');
    }

    function getTheme() {
        return window.localStorage.getItem('theme') ?? 'light';
    }

    function init() {
        document.getElementById('main-navbar').style.cssText = 'background-color: #36393d;';

        let theme = getTheme();
        if (theme) {
            changeTheme(theme);
        }
    }

    document.getElementById('ToolbarChangeTheme').addEventListener('click', () => {
        toggleTheme();
    });

    init();
});
