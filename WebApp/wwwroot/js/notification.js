document.addEventListener('DOMContentLoaded', () => {
	const connection = new signalR.HubConnectionBuilder()
		.withUrl("/notificationHub")
		.build()

	connection.on("ReceiveNotification", function (notification) {
		const container = document.querySelector(".notification-group")
		console.log("received")
		const item = document.createElement('div')
		item.className = "notification-item"
		item.setAttribute("data-id", notification.id)
		item.innerHTML =
		`
			<img ${notification.icon !== null ? "src=${notification.icon}" : ""} alt="" />
			<span class="message">${notification.message}</span>
			<span class="time" data-created="${new Date(notification.created).toISOString()}">${notification.created}</span>
			<button type="button" class="btn-close" onclick="dismissNotification("${notification.id}")"></button>
		`
		container.insertBefore(item, container.firstChild)

		updateRelativeTimes()
		updateNotificationCount()
	})

	connection.on("NotificationDismissed", function (id) {
		removeNotification(id)
	})

	connection.start().catch(error => console.error(error))
})

async function dismissNotification(id) {
	try {
		const res = await fetch(`/api/notifications/dismiss/${id}`, { method: "POST" })
		if (res.ok) {
			removeNotification(id)
		}
		else {
			console.error("Error removing notifiaction.")
		}
	}
	catch (error) {
		console.error("Error removing notifiaction: ", error)
	}
}

function removeNotification(id) {
	const element = document.querySelector(`.notification-item[data-id="${id}"]`)
	if (element) {
		element.remove()
		updateNotificationCount()
	}
}

function updateNotificationCount() {
	const notificationModalButton = document.querySelector("#notifications-btn")
	const counter = document.querySelector(".notification-counter")
	const notifications = document.querySelector(".notification-group")
	const count = notifications.children.length

	if (counter) {
		counter.textContent = count
	}
	if (notificationModalButton) {
		if (count > 0) {
			notificationModalButton.classList.add("not-empty")
		}
		else {
			notificationModalButton.classList.remove("not-empty")
		}
	}
}

function updateRelativeTimes() {
	console.log("implement me")
	//2.02.00
}