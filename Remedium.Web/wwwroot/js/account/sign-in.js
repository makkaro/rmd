void function() {
    const form = document.forms['rNavbarSignInForm']
    form.onsubmit = function(event) {
        event.preventDefault()
        const xhr = new XMLHttpRequest()
        xhr.open('POST', '/Account/SignIn')
        xhr.send(new FormData(form))
        xhr.onload = function() {
            if (xhr.status == 201) {
                location.reload()
            }
            alert(`${xhr.status}: ${xhr.response}\nPlease try again...`)
        }
    }
}()