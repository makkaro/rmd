void function() {
    new Array().slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]')).map(trigger => {
        return new bootstrap.Tooltip(trigger)
    })  
}()