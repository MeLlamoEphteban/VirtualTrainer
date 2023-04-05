import React, { useState, useEffect } from "react";
import { useNavigate } from 'react-router-dom';

function Row({item, deleteMe}) {
  const navigate = useNavigate();
  const [active, setActive] = useState(false);

  const handleEdit = (id) => {
      //EditExercise(item.idexercise);
      //setShowEdit(true);
      navigate("/Exercises/Edit/" + id);
  }

  function timeout(delay){
    return new Promise( res => setTimeout(res, delay));
  }

  const handleDelete = async (id) => {
    setActive(true);
    await timeout(300);
    const response = window.confirm("Are you sure you want to delete the item?");

    if(response){
        let res = fetch(`http://localhost:5000/Exercises/DeleteExercise?id=${id}`, {
          method: "POST"
        }).then ((res) => {
          
        if(res.status === 200)
        {
          window.alert("Selection deleted!");
          deleteMe(item);
        }
      }).catch((e) => {
        console.log("Error in fetch", e);
      })
    }
    else
    {
      setActive(false);
      window.alert("Action aborted!");
    }
  }
    return (
      <tr style={{backgroundColor: active ? "red" : "white"}}>
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

  const deleteChild = (child) => {
    var x = data.filter(value => value != child);
    setData(x);
  }

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
          <Row key={item.idexercise} item={item} deleteMe={deleteChild}/>
        ))}
      </tbody>
    </table>
    </>
    );
  }

export default MyComponent;