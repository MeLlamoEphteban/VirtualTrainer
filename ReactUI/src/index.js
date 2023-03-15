import React from 'react';
import ReactDOM from 'react-dom/client';
import Navbar from "./Navbar";
import "./styles.css";

function App() {
  return (
    <>
      <Navbar />
    </>
  )
}

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<App />);