document.addEventListener('DOMContentLoaded', async function () {
    const partialLoadElements = document.querySelectorAll('.partialload');

    const tasks = Array.from(partialLoadElements).map(async (element) => {
        const apiUrl = element.getAttribute('data-api');
        const params = element.getAttribute('data-params');

        if (apiUrl) {
            let url = apiUrl;
            if (params) {
                const separator = url.includes('?') ? '&' : '?';
                url += separator + params;
            }

            try {
                const response = await fetch(url);
                if (response.ok) {
                    element.innerHTML = await response.text();
                } else {
                    console.error('Error loading partial content');
                    element.innerHTML = '<p>Error loading content</p>';
                }
            } catch (error) {
                console.error('Request failed', error);
                element.innerHTML = '<p>Error loading content</p>';
            }
        }
    });

    await Promise.all(tasks); // همه با هم اجرا میشن
});
