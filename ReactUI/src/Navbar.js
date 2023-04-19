import { BrowserRouter, Route, Routes, Link } from 'react-router-dom';
import Home from './pages/Home';
import Exercises from './pages/Exercises';
import Users from './pages/Users';
import EditExercise from './editExercise';
import EditUser from './editUser';
import CreateExercise from './createExercise';
import CreateUser from './createUser';

export default function Navbar() {
    return (
        <>
        <BrowserRouter>
        <nav className="nav">
            <Link to="/Home" className='site-title'>Virtual Trainer</Link>
            <ul>
                <li><Link to="/Exercises">Exercises</Link></li>
                <li><Link to="/Users">Users</Link></li>
            </ul>
        </nav>
        <Routes>
            <Route path='/Home' element={<Home />}>Virtual Trainer</Route>
            <Route path='/Exercises' element={<Exercises />}>Exercises</Route>
            <Route path='/Users' element={<Users />}>Users</Route>
            <Route path='/Exercises/Edit/:exerciseID' element={<EditExercise />}></Route>
            <Route path='/Exercises/Create' element={<CreateExercise />}></Route>
            <Route path='/Users/Create' element={<CreateUser />}></Route>
            <Route path='/Users/Edit/:userID' element={<EditUser />}></Route>
        </Routes>
        </BrowserRouter>
        </>
    )
}