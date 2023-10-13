import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from 'react-query/devtools'
import { Swapi } from "./Swapi.js";

const queryClient = new QueryClient({});

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      
      <div className="App">
        <Swapi/>
      </div>

      {/* <ReactQueryDevtools initialIsOpen={true} /> */}

    </QueryClientProvider>
  );
};

export default App; 