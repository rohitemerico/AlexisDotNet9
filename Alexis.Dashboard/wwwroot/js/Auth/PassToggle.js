/* ---------------------------------------------------
    Toggle show/hide password
----------------------------------------------------- */
window.onload = function (e) {

    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#Password');

    togglePassword.addEventListener('click', function (e) {
        // toggle the type attribute
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);
        // toggle the eye / eye slash icon
        if (type === 'password') {
            this.classList.remove('fa-eye-slash');
            this.classList.add('fa-eye');
        }
        else {
            this.classList.add('fa-eye-slash');
            this.classList.remove('fa-eye');
        }
    });
}