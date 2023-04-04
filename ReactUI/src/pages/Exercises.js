import React, { useState, useEffect } from "react";
import { useNavigate } from 'react-router-dom';

function Row({item}) {
  const navigate = useNavigate();

  const handleEdit = (id) => {
      //EditExercise(item.idexercise);
      //setShowEdit(true);
      navigate("/Exercises/Edit/" + id);
  }

  const handleDelete = (id) => {
    //navigate("/Exercises/Delete/" + id);
    const response = window.confirm("Are you sure you want to delete the item?");

    if(response){
      try{
        let res = fetch(`http://localhost:5000/Exercises/DeleteExercise?id=${id}`, {
          method: "POST"
        });
        if(res.status === 200)
        {
          window.alert("Selection deleted!");
        }
      } catch(err){
        console.log(err);
      }
    }
    else
    {
      window.alert("Action aborted!");
    }
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
        <td>
          <button onClick={() => handleDelete(item.idexercise)}>Delete</button>
        </td>
      </tr>
      );
}

function MyComponent() {
  const [data, setData] = useState([]);
  const navigate = useNavigate();
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

    const handleCreate = () => {
      navigate("/Exercises/Create");
    }

    return (
      <>
      <h3 onClick={() => handleCreate()}>Create Exercise</h3>
      <table>
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Reps</th>
          <th>Sets</th>
          <th>Weight</th>
          <th>Action</th>
          <th>Action</th>
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