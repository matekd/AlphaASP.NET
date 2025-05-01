document.addEventListener("DOMContentLoaded", () => {
    const consentCookie = getCookie("consentCookie")

    if (!consentCookie) {
        showCookieModal()
    }
    else if (JSON.parse(consentCookie).functional) {
        const themeCookie = getCookie("ThemeCookie")
        if (themeCookie) {
            setTheme(JSON.parse(themeCookie).theme)
        }
        else {
            setCookie("ThemeCookie", JSON.stringify({ theme: 'light' }), 365)
        }
    }
    
    const darkmodeSwitch = document.querySelector('#darkmode')
    if (darkmodeSwitch) {
        darkmodeSwitch.checked = document.documentElement.getAttribute("data-theme") === "dark"
        darkmodeSwitch.addEventListener('input', () => {
            if (JSON.parse(getCookie("consentCookie")).functional) {
                setCookie("ThemeCookie", JSON.stringify({ theme: darkmodeSwitch.checked ? 'dark' : 'light' }), 365)
                setTheme(darkmodeSwitch.checked ? 'dark' : 'light')
            }
            else {
                setTheme("light")
                darkmodeSwitch.checked = false
            }
        })
    }
})

function setTheme(theme) {
    if (theme !== "dark" && theme !== "light")
        theme = "light"
    document.documentElement.setAttribute('data-theme', theme)
}

function showCookieModal() {
    const modal = document.getElementById("cookieModal")
    if (modal)
        modal.style.display = "flex"
    
    const consentValue = getCookie("consentCookie")
    if (!consentValue) return

    try {
        const consent = JSON.parse(consentValue)
        document.getElementById("cookieFunctional").checked = consent.functional
        //document.getElementById("cookieAnalytics").checked = consent.analytics
        //document.getElementById("cookieMarketing").checked = consent.marketing
    }
    catch (error) {
        console.error("Unable to handle cookie consent value. ", error)
    }
}

function hideCookieModal() {
    const modal = document.getElementById("cookieModal")
    if (modal)
        modal.style.display = "none"
}

function getCookie(name) {
    const nameEQ = name + "="
    const cookies = document.cookie.split(";")
    for (let cookie of cookies) {
        cookie = cookie.trim()
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    return null
}

function setCookie(name, value, days) {
    let expires = ""
    if (days) {
        const date = new Date()
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000)
        expires = "; expires=" + date.toUTCString()
    }

    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodedValue}${expires}; path=/; Samesite=Lax`
}

async function acceptAll() {
    const consent = {
        essential: true,
        functional: true,
        //analytics: true,
        //marketing: true
    }

    setCookie("consentCookie", JSON.stringify(consent), 365)
    await setConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById("cookieConsentForm")
    const formData = new FormData(form)

    const consent = {
        essential: true,
        functional: formData.get("functional") === "on",
        //analytics: formData.get("analytics") === "on",
        //marketing: formData.get("marketing") === "on"
    }
    setCookie("consentCookie", JSON.stringify(consent), 365)
    await setConsent(consent)
    hideCookieModal()

    if (!consent.functional)
        setTheme("light")
}

async function setConsent(consent) {
    try {
        const res = await fetch("/cookies/setcookies", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(consent)
        })

        if (!res.ok) {
            console.error("Failed to set cookie consent. ", await res.text())
        }
    }
    catch (error) {
        console.error("Error: ", error)
    }
}
