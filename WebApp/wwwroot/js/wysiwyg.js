document.addEventListener('DOMContentLoaded', () => {
	var editors = document.querySelectorAll(".wysiwyg-editor")
	editors.forEach(editor => {
		const textarea = editor.querySelector("textarea")
		const id = editor.getAttribute("data-id")
		const editorId = "#project-wysiwyg-editor-" + id
		const toolbarId = "#project-wysiwyg-toolbar-" + id
		const content = textarea.value !== "" ? textarea.value : "<p><br></p>"
		initWysiwyg(textarea, editorId, toolbarId, content)
	})
})

function initWysiwyg(textarea, editorId, toolbarId, content) {
	const quill = new Quill(editorId, {
		modules: {
			syntax: true,
			toolbar: toolbarId,
		},
		placeholder: "Type something",
		theme: "snow"
	})

	if (content)
		quill.root.innerHTML = content

	quill.on("text-change", () => {
		textarea.value = quill.root.innerHTML
	})
}