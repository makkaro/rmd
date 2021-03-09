void function() {
    const modal = document.getElementById('modal')
    const bsModal = new bootstrap.Modal(modal)
    
    modal.addEventListener('show.bs.modal', async event => {
        const { url, title } = event.relatedTarget.dataset

        const modalBody = modal.querySelector('.modal-body')
        modalBody.innerHTML = await (await fetch(url)).text()
        modal.querySelector('.modal-title').textContent = title

        const confirmationBtn = modal.querySelector('.modal-confirmation')
        confirmationBtn.onclick = async () => {
            const responseJson = await (await fetch(url, {
                method: 'POST',
                body: new FormData(modal.querySelector('.modal-form'))
            })).json()
            if (responseJson['valid']) {
                bsModal.hide()
                document.getElementById('viewAll').innerHTML = responseJson['viewResult']
            } else {
                modalBody.innerHTML = responseJson['viewResult']
                modal.dispatchEvent(new Event('r.load'))
            }
        }
        
        modal.addEventListener('r.load', event => {
            modal.querySelector('.modal-form').onsubmit = event => {
                event.preventDefault()
                confirmationBtn.dispatchEvent(new Event('click'))
            }
        })

        modal.dispatchEvent(new Event('r.load'))
    })
}()