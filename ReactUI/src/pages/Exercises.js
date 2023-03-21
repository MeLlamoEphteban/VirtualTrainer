import React, { useState, useEffect } from "react";
import { useNavigate } from 'react-router-dom';

function Row({item}) {
  const navigate = useNavigate();

  const handleEdit = (id) => {
      //EditExercise(item.idexercise);
      //setShowEdit(true);
      navigate("/Exercises/Edit/" + id);
  }
    return (
      <tr>
        <td>{item.exerciseName}</td>
        <td>{item.instructions}</td>
        <td>{item.reps}</td>
        <td>{item.sets}</td>
        <td>{item.weight}</td>
        <td>
          <button onClick={() => handleEdit(item.idexercise)}>Edit</button>
        </td>
      </tr>
      );
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
      <>
      <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Reps</th>
          <th>Sets</th>
          <th>Weight</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        {data.map(item => (
          <Row key={item.idexercise} item={item} />
        ))}
      </tbody>
    </table>
    </>
    );
  }

export default MyComponent;