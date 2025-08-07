'use client'

import { useRouter } from 'next/navigation';
import { useState } from "react"
import styles from "@/styles/recipts-page/hero/hero.module.css"


export default function Hero() {
    const router = useRouter();
    const [inputName, setInputName] = useState('');
    const mockResources = [
        {id:1, resourceName:'Ноутбуки111'}
    ]
    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурс</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={() => router.push('/resources-page')}>Сохранить</button>
                <button className={styles.deleteButton} onClick={() => router.push('/resources-page')}>Удалить</button>
                <button className={styles.acceptButton} onClick={() => router.push('/resources-page')}>В работу</button>
            </div>
            <div className={styles.inputGroup}>
                <h4 className={styles.filterText}>Наименование</h4>
                <input
                type='text'
                className={styles.inputNumber}
                value={inputName}
                onChange={(e) => setInputName(e.target.value)}
                />
            </div>
        </main>
    )
}