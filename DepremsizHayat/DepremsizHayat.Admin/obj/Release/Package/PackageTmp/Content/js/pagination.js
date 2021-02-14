function pagination(elementId, location) {
    var urlParams = new URLSearchParams(window.location.search);
    var p = urlParams.get('p')
    let element = document.querySelector(`#${elementId}`)
    var contentPerPage = element.dataset.contentPerpage
    var contentCount = element.dataset.contentCount

    if (!Number.isInteger(parseInt(p)) || parseInt(p) < 0) p = 1
    if (parseInt(p) > Math.ceil(contentCount / contentPerPage)) p = Math.ceil(contentCount / contentPerPage)

    



    let first = document.createElement("button")
    first.innerHTML = "<<"

    let previous = document.createElement("button")
    previous.innerHTML = "<"

    let pageSpan = document.createElement("span")
    pageSpan.innerHTML = `${p}/${Math.ceil(contentCount / contentPerPage)}`

    let next = document.createElement("button")
    next.innerHTML = ">"

    let last = document.createElement("button")
    last.innerHTML = ">>"

    if (p == 1) { previous.disabled = true; first.disabled = true }
    if (p == Math.ceil(contentCount / contentPerPage)) { next.disabled = true; last.disabled = true }

    first.onclick = function () {
        window.location = location + "?p=1"
    }
    previous.onclick = function () {
        window.location = location + "?p=" + (p - 1)
    }
    next.onclick = function () {
        window.location = location + `?p=${parseInt(p) + 1}`
    }
    last.onclick = function () {
        window.location = location + `?p=${Math.ceil(contentCount / contentPerPage)}`
    }

    element.appendChild(first)
    element.appendChild(previous)
    element.appendChild(pageSpan)
    element.appendChild(next)
    element.appendChild(last)
}
