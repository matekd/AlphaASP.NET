document.addEventListener('DOMContentLoaded', () => {
    const search = document.getElementById("search")
    search.addEventListener("input", () => {
        const term = search.value.toLowerCase()
        // an empty value, "", resets the filtering
        searchItems(term)
    })
})

function searchItems(search) {
    // list items has varying class names, therefore the we take the children of the parent element
    const itemList = document.querySelector(".item-list")
    // convert from HTMLCollection into something iterable
    const items = Array.from(itemList.children)

    items.forEach(item => {
        item.classList.remove("search-hide")

        if (item.getAttribute("data-search").toLowerCase().indexOf(search) === -1) {
            item.classList.add("search-hide")
        }
    })
}