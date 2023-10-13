import { useQuery } from 'react-query'
import {useState} from 'react';

export function Swapi() {
    const[value, setValue] = useState(""); 

    const { data, status, error, refetch } = useQuery('myData', async () => {
        let requestUrl = value;

        if (!requestUrl)
            requestUrl = 'https://localhost:7142/api/v1/swapi/films/1';

        let response = await fetch(requestUrl, {
            headers: {
                'Authorization': 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjI1ZjNjMDhiLWI4OWMtNDQ0MC1iNjBmLTM5ZTEyM2ZjZWUzYyIsInN1YiI6InNkIiwiZW1haWwiOiJzZCIsImp0aSI6IjJlYWM0ZmZiLWY2ZDgtNGM1MS1hZjU5LTRlY2VlZTRiNDE5YyIsIm5iZiI6MTY5NzE5NzkzMywiZXhwIjoxNjk3MTk4MjMzLCJpYXQiOjE2OTcxOTc5MzMsImlzcyI6Imh0dHBzOi8vaXNzdWVyLmNvbS8iLCJhdWQiOiJodHRwczovL2F1ZGllbmNlLmNvbS8ifQ.sX1cZTFBzyWlE-VsSEKj4fkGp1FWMDgFZMcIsoqwaqDHBDeIiWCT6wDmHrLoVAEKb2MLhN4Meij0CcOq0GxQEw',
            }
        });

        var body = await response.json();

        return body;
    })

    function handleChange(e) {
        setValue(e.target.value);
    }

    if (status === 'loading') {

        return <p>Loading...</p>

    }
    if (status === 'error') {

        return <p>Error: {error.message}</p>

    }
    return (
        <>
            <div><pre>{JSON.stringify(data, null, 2) }</pre></div>
            <input type="text" onChange={handleChange} />
            <button type="button" onClick={refetch}>
                Fetch again
            </button>
        </>
    )
}