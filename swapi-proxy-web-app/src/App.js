import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from 'react-query/devtools'
import { Swapi } from "./Swapi.js";

const queryClient = new QueryClient({});

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      

      <Swapi/>

      <ReactQueryDevtools initialIsOpen={true} />

    </QueryClientProvider>
  );
};

export default App;