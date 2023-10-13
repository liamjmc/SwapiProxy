import { useQuery } from 'react-query'

export function Swapi() {

  const { data, status, error, refetch } = useQuery('myData', async () => {

    const response = await fetch('https://swapi.dev/api/films/1/')

    return response.data

  })
  if (status === 'loading') {

    return <p>Loading...</p>

  }
  if (status === 'error') {

    return <p>Error: {error.message}</p>

  }
  return (
    <>
        <div>{data}</div>
        <button type="button" onClick={refetch}>
          Fetch again
        </button>
    </>
  )
}