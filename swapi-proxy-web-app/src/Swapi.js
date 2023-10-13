import {useState} from 'react';
import './Swapi.css';

export function Swapi() {
    const [url, setUrl] = useState('https://localhost:7142/api/v1/swapi/films/1')
    const [body, setBody] = useState('');
    const [data, setData] = useState(null);
    const [method, setMethod] = useState('GET');
    
    const handleClick = async () => {
        try {
            debugger;
            let response = await fetch(url, {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjI1ZjNjMDhiLWI4OWMtNDQ0MC1iNjBmLTM5ZTEyM2ZjZWUzYyIsInN1YiI6InNkIiwiZW1haWwiOiJzZCIsImp0aSI6IjJlYWM0ZmZiLWY2ZDgtNGM1MS1hZjU5LTRlY2VlZTRiNDE5YyIsIm5iZiI6MTY5NzE5NzkzMywiZXhwIjoxNjk3MTk4MjMzLCJpYXQiOjE2OTcxOTc5MzMsImlzcyI6Imh0dHBzOi8vaXNzdWVyLmNvbS8iLCJhdWQiOiJodHRwczovL2F1ZGllbmNlLmNvbS8ifQ.sX1cZTFBzyWlE-VsSEKj4fkGp1FWMDgFZMcIsoqwaqDHBDeIiWCT6wDmHrLoVAEKb2MLhN4Meij0CcOq0GxQEw',
                },
                method: method,
                body: method == 'GET' ? undefined : body
            });
    
            var data = await response.json();

            setData(data)
        } catch (err) {
            console.log(err.message)
        }
    }

    return (
        <>
            <div className="form-area">
                Url: <input type="text" value={url} onChange={e => setUrl(e.target.value)} /> <br />
                Method: 
                    <select value={method} onChange={e => setMethod(e.target.value)}>
                        <option value="GET">GET</option>
                        <option value="POST">POST</option>
                    </select> <br/>
                Body: <textarea value={body} onChange={e => setBody(e.target.value)} /> <br />
                <button type="button" onClick={handleClick}>
                    Fetch
                </button>
            </div>
            <div><pre>{JSON.stringify(data, null, 2) }</pre></div>
        </>
    )
}