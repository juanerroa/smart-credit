const postRequest = async (url, objeto) => {
    try {
        const DATA = JSON.stringify(objeto);
        const respuesta = await fetch(url, {
            method: 'POST',
            body: DATA,
            headers: {
                '__RequestVerificationToken': requestVerificationToken,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
        });

        if (!respuesta.ok) {
            throw new Error(`Error en la solicitud: ${respuesta.status} - ${respuesta.statusText}`);
        }

        const response = await respuesta.json();
        return response;
    } catch (error) {
        console.error('Error al realizar la solicitud POST:', error);
        throw error;
    }
}

const renderPartial = async (url, objeto) => {
    const DATA = JSON.stringify(objeto);

    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: 'POST',
            dataType: "html",
            contentType: 'application/json',
            data: DATA,
            success: function (response) {
                resolve(response);
            },
            error: function (xhr, status, error) {
                reject(error);
            }
        });
    });
}