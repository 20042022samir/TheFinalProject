const filterBtn = document.getElementById("filterButton");
const filterModal = document.getElementById("filterModal");
const modalCloseBtn = document.getElementById("modalClose");
const hamburGerOpenBtn = document.getElementById("hamBurgerBtns");
const phoneModal = document.getElementById("phoneModal");
const phoneCloseBtn = document.getElementById("phoneModalCloseBtn");

filterModal.style.display = "none"
filterBtn.addEventListener("click", () => {
  filterModal.style.display = 'flex'
})
modalCloseBtn.addEventListener("click", () => {
  filterModal.style.display = 'none'
})


hamburGerOpenBtn.addEventListener("click", () => {
  phoneModal.style.top = '0'
})

phoneCloseBtn.addEventListener("click", () => {
  phoneModal.style.top = '-300px'
})

