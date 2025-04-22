
document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150

    // open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)
            
            if (modal)
                modal.style.display = 'flex';
        })
    })

    // close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')

            if (modal) {
                modal.style.display = 'none'

                modal.querySelectorAll('form').forEach(form => {
                    form.reset()
                    const imagePreview = document.querySelector('.image-preview')
                    const imagePreviewer = imagePreview.closest('.image-previewer')
                    if (imagePreview)
                        imagePreview.src = ""

                    if (imagePreviewer)
                        imagePreviewer.classList.remove('selected')
                })
            }
        })
    })

    // toggle modal
    const toggleButtons = document.querySelectorAll('[data-toggle="true"]')
    toggleButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)
            if (modal) {
                button.classList.toggle("active")
                modal.classList.toggle("show")
            }
        })
    })

    // handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type=file]')
        const imagePreview = previewer.querySelector('.image-preview')

        previewer.addEventListener('click', () => fileInput.click())
        
        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })

    // handle form submit
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        // Authenitaction forms return a view instead of request, this is needed for returnUrl and redirections to work
        if (!form.classList.contains('auth-form')) {
            form.addEventListener('submit', async (e) => {
                e.preventDefault()
                clearErrorMessages(form)
                const formData = new FormData(form)
                const method = form.getAttribute("submitMethod") ?? "post"
                try {
                    const res = await fetch(form.action, {
                        method: method,
                        body: formData,
                    })

                    if (res.ok) {
                        const modal = form.closest('.modal')
                        if (modal)
                            modal.style.display = "none";

                        window.location.reload();
                    }
                    else if (res.status === 400) {
                        const data = await res.json()
                        if (data.errors) {
                            Object.keys(data.errors).forEach(key => {
                                const input = form.querySelector(`[name="${key}"]`)
                                if (input) {
                                    input.classList.add('input-validation-error')
                                }

                                const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                                if (span) {
                                    span.innerText = data.errors[key].join('\n')
                                    span.classList.add('field-validation-error')
                                    span.classList.remove('field-validation-valid')
                                }
                            })
                        }
                        if (data.submitError) {
                            const span = form.querySelector('.submit-error')
                            if (span) {
                                span.innerText = data.submitError
                                span.classList.add('field-validation-error')
                            }
                        }
                    }
                }
                catch {
                    console.error('error submitting the form')
                }
            })
        }
        form.addEventListener('change', () => {
            const span = form.querySelector('.submit-error')
            if (span) {
                span.innerText = ''
                span.classList.remove('field-validation-error')
            }
        })
    })

    // dark mode
    // expand on later
    const btn = document.querySelector('#darkmode')
    if (btn)
        btn.addEventListener('click', () => {
            document.documentElement.setAttribute('data-theme', 'dark')
        })
})

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val=true]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
        span.classList.add('field-validation-valid')
    })
}

async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()
        reader.onerror = () => reject(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load image."))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }
        reader.readAsDataURL(file)
    })
}

async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize
        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error("Failed on image processing: ", error)
    }
}