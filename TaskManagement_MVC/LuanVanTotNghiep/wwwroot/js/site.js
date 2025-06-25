
document.getElementById("iconMenu").addEventListener("click", function () {
    let sidebar = document.getElementById("sidebar");
    let navbar = document.querySelector(".navbar");

    if (sidebar.style.width === "250px" || sidebar.style.width === "") {
        sidebar.style.width = "60px"; // Thu nhỏ sidebar
        navbar.style.marginLeft = "60px";
        container_id.style.marginLeft = "60px"// Dịch navbar vào gần hơn
        sidebar.classList.add("collapsed"); 
    } else {
        sidebar.style.width = "250px"; // Mở rộng sidebar
        navbar.style.marginLeft = "250px";
        container_id.style.marginLeft = "250px";// Dịch navbar ra xa hơn
        sidebar.classList.add("collapsed"); 
        sidebar.classList.remove("collapsed");
    }
});
document.getElementById("switch").addEventListener("change", function () {
    const linkText = document.getElementById("modeText");
    if (this.checked) {
        // Khi checkbox được chọn: chuyển background thành tối
        document.body.style.backgroundColor = "#333"; // màu nền tối
        document.body.style.color = "#fff"; // đổi màu chữ cho dễ đọc
        linkText.textContent = "Dark";
    } else {
        // Khi checkbox không được chọn: chuyển background về sáng
        document.body.style.backgroundColor = "#fff"; // màu nền sáng
        document.body.style.color = "#000"; // màu chữ mặc định
        linkText.textContent = "Light";
    }
});

