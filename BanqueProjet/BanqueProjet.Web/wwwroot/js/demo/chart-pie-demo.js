window.addEventListener('load', function () {
    console.log('chart-pie-demo.js chargé, test du canvas…');
    var canvas = document.getElementById('myPieChart');
    console.log('canvas trouvé ? ?', canvas);
    if (!canvas) return;  // pas de canvas, on stoppe

    // Exemple : vos données viendront peut-être d’une variable JS générée par Razor
    // ou d’un appel AJAX. Ici on simule un tableau vide pour l’instant :
    var chartData = {
        labels: [],      // aucun label pour l’instant
        datasets: []     // aucun dataset pour l’instant
    };

    // Si pas de données, on affiche juste le message “noDataMessage” et on arrête.
    if (!chartData.datasets.length) {
        var msg = document.getElementById('noDataMessage');
        if (msg) msg.style.display = 'block';
        return;
    }

    // Sinon, on cache le message et on construit le chart
    var msg = document.getElementById('noDataMessage');
    if (msg) msg.style.display = 'none';

    var ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'pie',
        data: chartData,
        options: { /* vos options */ }
    });
});
