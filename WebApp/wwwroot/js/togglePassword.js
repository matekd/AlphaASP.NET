document.addEventListener("DOMContentLoaded", () => {
    togglePasswordVisiblityButtons = document.querySelectorAll(".eye")
    togglePasswordVisiblityButtons.forEach(toggle => {
        toggle.addEventListener("click", () => {
            const input = toggle.parentElement.querySelector("input")
            if (input.getAttribute("type") === "password") {
                input.setAttribute("type", "text")
                toggle.classList.add("closed")
            }
            else {
                input.setAttribute("type", "password")
                toggle.classList.remove("closed")
            }
        })
    })
})