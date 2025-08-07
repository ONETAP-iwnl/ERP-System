'use client'

import { useRouter } from 'next/navigation';
import { useState } from 'react';
import { createResource } from '@/services/api/resourceClient';
import styles from '@/styles/recipts-page/hero/hero.module.css';
import { Asap } from 'next/font/google';


export default function Hero() {
    const [inputName, setInputName] = useState('');
    const router = useRouter();

    const handleSave = async () => {
        try{
            await createResource(inputName);
            router.push('/resources-page');
        } catch(error) {
            console.error('Ошибка при сохрании:', error);
            alert('Не удалось создать единицу измерения.');
        }
    }

    return(
        <main className={styles.mainContainer}>
            <h1 className={styles.headerText}>Ресурс</h1>
            <div className={styles.buttonGroup}>
                <button className={styles.addButton} onClick={handleSave}>Сохранить</button>
            </div>
            <div className={styles.filterBox}>
                <h4 className={styles.filterText}>Наименование</h4>
                <div className={styles.inputGroup}>
                    <input
                    type='text'
                    className={styles.inputNumber}
                    value={inputName}
                    onChange={(e) => setInputName(e.target.value)}
                    />
                </div>
            </div>
        </main>
    )
}