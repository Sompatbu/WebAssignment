import { useEffect, useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
    const [forecasts, setForecasts] = useState();
    const [file, setFile] = useState(null);

    const onFileChange = event => {
        setFile(event.target.files[0]);
    };

    const onFileUpload = () => {
        const formData = new FormData();
        formData.append("file", file);

        axios.post("https://localhost:49321/transaction/upload", formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        })
            .then(response => alert('File uploaded successfully: ' + response.data))
            .catch(error => alert('Error uploading file: ' + error));
    };

    useEffect(() => {
        populateTransactionData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Payment</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map(forecast =>
                    <tr key={forecast.id}>
                        <td>{forecast.id}</td>
                        <td>{forecast.payment}</td>
                        <td>{forecast.status}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tableLabel">Transaction</h1>
            <h1>File Uploader</h1>
            <input type="file" onChange={onFileChange} />
            <button onClick={onFileUpload}>
                Upload!
            </button>

            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
    
    async function populateTransactionData() {
        const response = await fetch('transaction');
        if (response.ok) {
            const data = await response.json();
            setForecasts(data.data);
        }
    }
}

export default App;