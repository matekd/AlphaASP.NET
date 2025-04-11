function filterProjects(status) {
    const projects = document.querySelectorAll(".project-card")
    if (status === "All")
        for (p of projects)
            p.classList.remove("hide")
    else
        for (p of projects) {
            if (p.getAttribute("status") === status)
                p.classList.remove("hide")
            else
                p.classList.add("hide")
        }

    const buttons = document.querySelectorAll(".tab-link")
    for (b of buttons) {
        b.classList.remove("active")
    }
    
    const activeButton = document.querySelector(`#${status}`)
    if (activeButton)
        activeButton.classList.add("active")
}