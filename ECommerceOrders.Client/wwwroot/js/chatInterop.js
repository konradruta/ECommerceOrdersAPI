// Umieść ten plik w: wwwroot/js/chatInterop.js
export function scrollToBottom(element) {
    element?.scrollIntoView({ behavior: "smooth", block: "end" });
}
