import { QueryClient, QueryClientProvider } from "react-query";
import { Swapi } from "./Swapi.js";

const queryClient = new QueryClient({});

const App = () => {
  return (
      <div className="App">
        <div className="App-header">
          <h1>Swapi API tester</h1>
        </div>
        <Swapi/>
      </div>
  );
};

export default App; 