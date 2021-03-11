void function() {
    document.getElementById('rNavbarSignOutLink').onclick = function() {
        const xhr = new XMLHttpRequest()
        xhr.open('DELETE', 'Account/SignOut')
        xhr.send()
        xhr.onload = function() {
            if (xhr.status == 204) {
                location.reload()
            }
            alert(`${xhr.status}: ${xhr.response}`)
        }
    } 
}()