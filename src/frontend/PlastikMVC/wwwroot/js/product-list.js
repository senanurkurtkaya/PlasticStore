document.addEventListener("DOMContentLoaded", function () {
    const searchBox = document.getElementById("searchBox");
    const productContainer = document.getElementById("productContainer");
    const productCards = document.querySelectorAll(".product-card");

    if (!searchBox) {
        console.error("Arama kutusu bulunamadı! (id='searchBox' eksik olabilir.)");
        return;
    }
    if (!productContainer) {
        console.error("Ürün konteyneri bulunamadı! (id='productContainer' eksik olabilir.)");
        return;
    }
    if (productCards.length === 0) {
        console.warn("Hiç ürün kartı bulunamadı! ('.product-card' eksik olabilir.)");
        return;
    }

    // Arama kutusuna göre filtreleme
    searchBox.addEventListener("input", function () {
        let searchText = searchBox.value.trim().toLowerCase();

        productCards.forEach(card => {
            let productName = card.getAttribute("data-name")?.toLowerCase() || "";
            if (productName.includes(searchText)) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    });

    // Ürün görsellerine tıklayınca modal açma işlemi
    const productImages = document.querySelectorAll(".product-img");

    productImages.forEach(imgDiv => {
        const imageContainer = imgDiv.querySelector("div[data-image-src]");

        if (imageContainer) {
            imgDiv.addEventListener("click", function () {
                openImageModal(imageContainer.getAttribute("data-image-src"));
            });
        }
    });
});

// Modal açma fonksiyonu
function openImageModal(imageUrl) {
    console.log("imageurl", imageUrl);
    const previewImage = document.getElementById("previewImage");
    const previewModal = document.getElementById("previewModal");

    if (previewImage && previewModal) {
        previewImage.src = imageUrl;
        new bootstrap.Modal(previewModal).show();
    } else {
        console.error("Ön izleme modalı veya resmi bulunamadı! (id='previewImage' ve id='previewModal' kontrol edin.)");
    }
}


