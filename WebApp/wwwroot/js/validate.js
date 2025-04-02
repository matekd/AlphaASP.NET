﻿document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector("form")
    if (!form) return

    const fields = form.querySelectorAll("input[data-val='true']")
    if (!fields) return

    fields.forEach(field => {
        field.addEventListener("input", () => {
            validateField(field)
        })
    })
})

function validateField(field) {
    let errorSpan = document.querySelector(`span[data-valmsg-for="${field.name}"]`)
    if (!errorSpan) return

    let errorMessage = ""
    let value = field.value.trim()

    if (field.hasAttribute("data-val-required") && value === "")
        errorMessage = field.getAttribute("data-val-required")

    else if (field.hasAttribute("data-val-regex") && value !== "") {
        let pattern = new RegExp(field.getAttribute("data-val-regex-pattern"))
        if (!pattern.test(value))
            errorMessage = field.getAttribute("data-val-regex")
    }

    if (errorMessage) {
        field.classList.add("input-validation-error")
        errorSpan.classList.remove('field-validation-valid')
        errorSpan.classList.add('field-validation-error')
        errorSpan.innerText = errorMessage
    }
    else {
        field.classList.remove("input-validation-error")
        errorSpan.classList.add('field-validation-valid')
        errorSpan.classList.remove('field-validation-error')
        errorSpan.innerText = ""
    }
}
