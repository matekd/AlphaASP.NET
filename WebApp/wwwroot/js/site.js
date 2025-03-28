﻿
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

    // handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type=file]')
        const imagePreview = previewer.querySelector('.image-preview')

        //using label
        //previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })

    // handle form submit
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)

            try {
                //Method not allowed without credentials
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData,
                    credentials: 'include'
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
                console.log('error submitting the form')
            }
        })
        form.addEventListener('change', async (e) => {
            var id = e.target.id
            if (id) {
                const span = form.querySelector(`[data-valmsg-for="${id}"]`)
                if (span) {
                    span.innerText = ''
                    span.classList.remove('field-validation-error')
                    span.classList.add('field-validation-valid')
                }
            }
            const span = form.querySelector('.submit-error')
            if (span) {
                span.innerText = ''
                span.classList.remove('field-validation-error')
            }
        })
    })

    // dark mode
    // expand on later
    var btn = document.querySelector('#darkmode')
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
    return new Promise((resolve, rejcet) => {
        const reader = new FileReader()

        reader.onerror = () => rejcet(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => rejcet(new Error("Failed to load image."))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }
        reader.readAsDataURL(file)
    })
}

async function processImage(file, imagePreviewer, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreviewer.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (errpr) {
        console.error("Failed on image processing: ", error)
    }
}