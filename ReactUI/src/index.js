import React, { useState, useEffect } from "react";
import ReactDOM from 'react-dom/client';
import EditExercise from "./editExercise";

function MyComponent() {
  const [error, setError] = useState(null);
  const [isLoaded, setIsLoaded] = useState(false);
  const [items, setItems] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5000/Exercises/GetExercisesRaw")
      .then(res => res.json())
      .then(
        (result) => {
          setIsLoaded(true);
          setItems(result);
        },
        (error) => {
          setIsLoaded(true);
          setError(error);
        }
      )
  }, [])

  if (error) {
    return <div>Error: {error.message}</div>;
  } else if (!isLoaded) {
    return <div>Loading...</div>;
  } else {
    return (
      <ul>
        {items.map(item => (
          <li key={item.idexercise}>
            {item.idexercise} {item.exerciseName}
          </li>
        ))}
      </ul>
    );
  }
}


const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<MyComponent />);