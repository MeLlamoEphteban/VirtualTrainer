import React, { useState, useEffect } from "react";
import ReactDOM from 'react-dom/client';
import EditExercise from "./editExercise";

function Row({item}) {
  const [showEdit, setShowEdit] = useState(false);
  const handleEdit = () => {
      //EditExercise(item.idexercise);
      setShowEdit(true);
  }
  if(!showEdit)
  {
    return (
      <tr>
        <td>{item.exerciseName}</td>
        <td>{item.instructions}</td>
        <td>
          <button onClick={handleEdit}>Edit</button>
        </td>
      </tr>
      );
  }else{
    return (
      <tr>
        <td colSpan={3}><EditExercise id={item.idexercise} handleEdit={setShowEdit}/></td>
      </tr>
    )
  }
}

function MyComponent() {
  const [data, setData] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5000/Exercises/GetExercisesRaw")
      .then(res => res.json())
      .then(
        (result) => {
          setData(result);
        },
        (error) => {
          console.error(error);
        }
      )
  }, [])

    return (
      <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {data.map(item => (
          <Row key={item.idexercise} item={item} />
        ))}
      </tbody>
    </table>
    );
  }

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<MyComponent />);