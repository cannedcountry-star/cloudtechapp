document.addEventListener("DOMContentLoaded", () => {
  const cards = document.querySelectorAll(".card");

  cards.forEach(card => {
    card.addEventListener("click", (e) => {
      e.preventDefault();
      const url = card.getAttribute("data-url");
      if (url) {
        if (typeof chrome !== "undefined" && chrome.tabs && chrome.tabs.create) {
          chrome.tabs.create({ url: url });
        } else {
          window.open(url, "_blank");
        }
      }
    });
  });
});
