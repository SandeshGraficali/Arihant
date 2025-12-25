
function isValidEmail(email) {
    email = email.trim().toLowerCase();
    var emailRegex = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.(com|in|org|net|co\.in)$/;
    return emailRegex.test(email);
}

function validateEmailField(emailSelector) {
    let email = $(emailSelector).val();

    if (!isValidEmail(email)) {
        Swal.fire({
            toast: true,
            position: 'bottom-end',
            icon: 'warning',
            title: 'Invalid Email',
            text: 'Please enter a valid email address (e.g., example@gmail.com)',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            customClass: {
                popup: 'my-swal-padding'
            }
        });
        return false;
    }

    return true;
}
