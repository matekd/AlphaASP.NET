document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector("form")
    if (!form) return

    const inputs = form.querySelectorAll("input[data-val='true']")
    if (inputs)
        inputs.forEach(input => {
            input.addEventListener("input", () => {
                validateField(input)
            })
        })
    const selects = form.querySelectorAll("select[data-val='true']")
    if (selects)
        selects.forEach(select => {
            select.addEventListener("input", () => {
                validateField(select)
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
